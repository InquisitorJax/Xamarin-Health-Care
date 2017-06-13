using Autofac;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Core
{
    public class XFormsNavigationService : INavigationService
    {
        //NOTE: Check out prism PageNavigationService class for sample XForms navigation samples https://github.com/PrismLibrary/Prism/tree/master/Source/Xamarin/Prism.Forms/Navigation
        //... or https://github.com/XLabs/Xamarin-Forms-Labs/blob/master/src/Forms/XLabs.Forms/Services/NavigationService.cs
        //... or https://github.com/jamesmontemagno/Hanselman.Forms/blob/master/Hanselman.Portable/Views/RootPage.cs
        //TODO: Take into account NavigationPage / TabbedPage / CarouselPage / MasterDetailPage ??

        private Page _currentPage;
        private bool _isSuspended;
        private MasterDetailPage _masterDetailPage;

        private Dictionary<Page, bool> _pageInfoList;
        private Stack<IView> _viewStack;

        public XFormsNavigationService()
        {
            _viewStack = new Stack<IView>();
            _pageInfoList = new Dictionary<Page, bool>();
        }

        public object Current
        {
            get { return _currentPage; }
        }

        private Page CurrentPage
        {
            get
            {
                if (_currentPage == null)
                    _currentPage = Application.Current.MainPage;
                return _currentPage;
            }
        }

        private IView CurrentView
        {
            get
            {
                //NOTE: will be null if the application's MainPage shell (usually a NavigationPage instance)
                return CurrentPage as IView;
            }
        }

        public async Task GoBack()
        {
            if (_isSuspended)
            {
                //GetType().GetLogger().Info("Navigation.GoBack while suspended");
                return;
            }

            await PopCurrentPageAsync();
            if (_viewStack.Count > 0)
                _currentPage = (Page)_viewStack.Peek();
        }

        public async Task NavigateAsync(string destination, Dictionary<string, string> args = null, bool modal = false, bool forgetCurrentPage = false, bool resetNavigation = false)
        {
            if (_isSuspended)
            {
                //GetType().GetLogger().Info("Navigation.Navigate while suspended");
                return;
            }
            IView view = ResolveView(destination);

            //await ShowViewAsync(view, modal, forgetCurrentPage); //Show View first so that showing doesn't wait for state initialization

            IViewModel viewModel = await ResolveViewModelAsync(destination, args);

            //NOTE: Views must still load their own viewmodels - since they could be hosted in a parent view
            view.ViewModel = viewModel;
            ((Page)view).BindingContext = viewModel;

            await ShowViewAsync(view, modal, forgetCurrentPage, resetNavigation);
        }

        public async Task ResumeAsync()
        {
            if (!_isSuspended)
                return;

            _isSuspended = false;
            if (CurrentView != null)
            {
                await CurrentView.ViewModel.LoadStateAsync();
            }
        }

        public async Task SuspendAsync()
        {
            if (_isSuspended)
                return;

            _isSuspended = true;
            if (CurrentView != null)
            {
                await CurrentView.ViewModel.SaveStateAsync();
            }
        }

        private void MakeMasterDetailRootPage(MasterDetailPage masterDetailPage)
        {
            _masterDetailPage = masterDetailPage;
            if (_masterDetailPage.Detail.GetType() != typeof(NavigationPage))
                throw new InvalidOperationException("MasterDetail.Detail must be a navigation page");
            Application.Current.MainPage = masterDetailPage;
            _currentPage = masterDetailPage.Detail; //the current page is where navigation happens, so current page is the root page
        }

        private void Page_Disappearing(object sender, EventArgs e)
        {
            if (_isSuspended)
            {
                //GetType().GetLogger().Info("Navigation.Page_Disappearing while suspended - view not popped");
                return;
            }

            IView view = sender as IView;

            if (view != null)
            {
                if (_viewStack.Count > 0 && _viewStack.Peek() == view) //ie: View has not been popped from local stack yet
                {
                    //hard Back button was pressed, or back navigation on nav bar was pressed (ie: Navigate was not called on navigation service)
                    PopViewStack();
                    if (_viewStack.Count > 0)
                    {
                        _currentPage = (Page)_viewStack.Peek();
                    }
                    else
                    {
                        //TODO: Log.Error: multiple validation requests for the same view
                    }
                }
            }
        }

        private async Task PopCurrentPageAsync()
        {
            if (CurrentPage != null)
            {
                bool isModal = false;
                if (_pageInfoList.ContainsKey(_currentPage))
                {
                    isModal = _pageInfoList[_currentPage];
                }

                //NOTE: pop view stack before calling popping navigation on page to cater for Page_Disappearing check
                PopViewStack();

                var navigationPage = _currentPage;
                if (_masterDetailPage != null)
                {
                    navigationPage = _masterDetailPage.Detail;
                }

                if (isModal)
                {
                    var page = await navigationPage.Navigation.PopModalAsync(true);
                }
                else
                {
                    await navigationPage.Navigation.PopAsync(true);
                }
            }
        }

        private IView PopViewStack()
        {
            IView poppedView = null;

            if (_viewStack.Count > 0 && _viewStack.Peek() == CurrentView)
            {
                poppedView = _viewStack.Pop();
                if (poppedView.ViewModel != null)
                {
                    poppedView.ViewModel.Closing();
                }
            }
            if (_pageInfoList.ContainsKey(_currentPage))
            {
                _pageInfoList.Remove(_currentPage);
            }

            return poppedView;
        }

        private async Task ResetNavigation(Page newRootPage)
        {
            //NOTE: if _masterDetailPage is root, then _currentPage will be _masterDetailPage.Detail
            if (CurrentPage != null && CurrentPage.Navigation.NavigationStack.Count > 0)
            {
                await CurrentPage.Navigation.PopToRootAsync();
            }

            _viewStack.Clear();
            _pageInfoList.Clear();
            _masterDetailPage = null;
            _currentPage = null;

            Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
            {
                Application.Current.MainPage = new NavigationPage(newRootPage);
            });

            _viewStack.Push((IView)newRootPage);
            _pageInfoList.Add(newRootPage, false);
        }

        private IView ResolveView(string destination)
        {
            IView view = CC.IoC.ResolveNamed<IView>(destination);

            return view;
        }

        private Task<IViewModel> ResolveViewModelAsync(string destination, Dictionary<string, string> args = null)
        {
            IViewModel viewModel = CC.IoC.ResolveNamed<IViewModel>(destination);
            viewModel.InitializeAsync(args); //don't await initialization - run purposefully async
            return Task.FromResult(viewModel);
        }

        private async Task ShowViewAsync(IView view, bool modal, bool forgetCurrentPage, bool resetNavigation)
        {
            if (resetNavigation && CurrentPage != null) //NOTE: Check current page for existing stack. If currentpage is null, then navigation is already reset
            {
                await ResetNavigation((Page)view); //typically when auth has failed and app needs to start from auth page again
                _currentPage = view as Page;
                return;
            }

            //NOTE: Add view to stack first, so disappearing when moving forward will not remove view from stack
            _viewStack.Push(view);
            _pageInfoList.Add((Page)view, modal);

            var masterDetailPage = view as MasterDetailPage; //NOTE: iOS - the MasterDetailPage must be the root, and the detail must be NavigationPage
            if (masterDetailPage != null)
            {
                //Replace the current root page with a master detail root page
                if (CurrentPage != null)
                    await PopCurrentPageAsync();
                MakeMasterDetailRootPage(masterDetailPage);
            }
            else if (CurrentPage == null)
            {
                //first time navigation - most likely Auth page if it's wasn't a master detail page
                Application.Current.MainPage = new NavigationPage((Page)view);
            }
            else
            {
                var page = (Page)view;
                page.Disappearing += Page_Disappearing;
                if (modal)
                {
                    await CurrentPage.Navigation.PushModalAsync((Page)view);
                }
                else
                {
                    var navigation = DependencyService.Get<INavigation>();

                    if (forgetCurrentPage)
                    {
                        CurrentPage.Navigation.InsertPageBefore((Page)view, CurrentPage);

                        await PopCurrentPageAsync();
                    }
                    else
                    {
                        if (_masterDetailPage != null)
                        {
                            await ((NavigationPage)_masterDetailPage.Detail).Navigation.PushAsync(page);
                        }
                        else
                        {
                            await CurrentPage.Navigation.PushAsync(page);
                        }
                    }
                }
            }

            _currentPage = view as Page;
        }
    }
}