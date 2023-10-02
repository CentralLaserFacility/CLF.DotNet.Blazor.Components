function translateTooltip(element) {
  var rect = element.getBoundingClientRect();
  if (rect.right >= (window.innerWidth || document.documentElement.clientWidth)) {
    element.style.left = `calc(${rect.left - rect.width}px - var(--tooltip-offset-x))`;
  }
  if (rect.bottom >= (window.innerHeight || document.documentElement.clientHeight)) {
    element.style.top = `calc(${rect.top - rect.height}px - var(--tooltip-offset-y))`;
  }
}