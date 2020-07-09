const vueLoader = require("../loaders/vue-loader");

module.exports = () => {
  return {
    test: /\.vue$/,
    use: [vueLoader()]
  };
};
