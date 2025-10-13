window.scrollToBottom = (element) => {
  if (element && element.scrollHeight) {
    element.scrollTop = element.scrollHeight;
  }
};
