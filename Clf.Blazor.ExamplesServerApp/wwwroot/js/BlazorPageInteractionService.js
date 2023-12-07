var windows = {}; //Global Variable

window.openNewWindowTab = (url, windowNameString) => {

  // Can NOT return to C# because window.open returns a WindowProxy object
  // WindowProxy is not JSON-serializable, so can't be used as a return value to .NET code.
  // https://stackoverflow.com/questions/62769031/how-can-i-open-a-new-window-without-using-js

  if (windowNameString in windows &&
    windows[windowNameString] &&
    windows[windowNameString].closed == false) {
    windows[windowNameString].focus();
  }
  else
    windows[windowNameString] = window.open(url, windowNameString);

  if (windows[windowNameString] == null)
    return "Window wasn't allowed to open.";
  else
    return "Window is open.";

};

window.closeWindowTab = (windowNameString) => {

  if (windowNameString in windows &&
    windows[windowNameString] &&
    windows[windowNameString].closed == false) {
    windows[windowNameString].close();
    delete windows[windowNameString];
  }
};

window.closeAllTabs = () => {
  for (const openedWindowName in windows) {
    window.closeWindowTab(openedWindowName);
  }
};

window.addEventListener('unload', function (event) {
  if (window.parent == window) {
    window.closeAllTabs();
  }
});
