module.exports = function (handler, timeout = 10000) {
  return new Promise((res, rej) => {
    const timer = setTimeout(() => {
      rej("Timeout reached");
    }, timeout);
    handler(() => {
      clearTimeout(timer);
      res();
    });
  });
};
