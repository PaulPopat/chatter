module.exports = function (time) {
  return new Promise(res => setTimeout(res, time));
}