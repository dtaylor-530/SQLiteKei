using SQLite.ViewModel;
using Utility.Common.Contracts;
using Utility.Entity;
using Utility.ViewModel;

namespace Utility.Common;

public class AboutViewModelKey : Key<IAboutViewModel> { }

public class PreferencesViewModelKey : Key<IPreferencesViewModel> { }

public class MainMenuViewModelKey : Key<IMainMenuViewModel> { }

public class MainWindowViewModelKey : Key<IMainWindowViewModel> { }

public class UnhandledExceptionViewModelKey : Key<IUnhandledExceptionViewModel> { }

public class MenuPanelViewModelKey : Key<IMenuPanelViewModel> { }

public class StatusViewModelKey : Key<IStatusViewModel> { }

public class TreeViewModelKey : Key<ITreeViewModel> { }

public class TabPanelViewModelKey : Key<ITabPanelViewModel> { }
