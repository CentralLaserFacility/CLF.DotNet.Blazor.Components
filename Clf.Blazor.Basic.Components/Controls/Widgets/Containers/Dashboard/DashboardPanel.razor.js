/*
 * https://css-tricks.com/piecing-together-approaches-for-a-css-masonry-layout/
 * https://codepen.io/chriscoyier/pen/WNGQyvX
 * 
 */

function resizeDashboardPane(item) {
  grid = document.getElementsByClassName("dashboard-panel-grid")[0];
  content = item.querySelector('.dashboard-pane');
  contentWidth = content.getBoundingClientRect().width;
  contentHeight = content.getBoundingClientRect().height;


  rowHeight = parseInt(window.getComputedStyle(grid).getPropertyValue('grid-auto-rows'));
  rowGap = parseInt(window.getComputedStyle(grid).getPropertyValue('grid-row-gap'));
  rowSpan = Math.ceil((contentHeight + parseInt(window.getComputedStyle(content).getPropertyValue('margin-top'))
                        + parseInt(window.getComputedStyle(content).getPropertyValue('margin-bottom')) + rowGap)
                        / (rowHeight + rowGap));
  item.style.gridRowEnd = "span " + rowSpan;

  colWidth = 2;
  colGap = parseInt(window.getComputedStyle(grid).getPropertyValue('grid-column-gap'));
  colSpan = Math.ceil((contentWidth + parseInt(window.getComputedStyle(content).getPropertyValue('margin-left'))
                       + parseInt(window.getComputedStyle(content).getPropertyValue('margin-right')) + colGap)
                      / (colWidth + colGap));
  item.style.gridColumnEnd = "span " + colSpan;
}

function resizeDashboardToMasonaryLayout() {
  allItems = document.getElementsByClassName("dashboard-pane-wrapper");
  for (x = 0; x < allItems.length; x++) {
    resizeDashboardPane(allItems[x]);
  }
}

window.addEventListener("resize", resizeDashboardToMasonaryLayout);