const cssLoader = require("../loaders/css-loader");
const postcssLoader = require("../loaders/postcss-loader");
const vueStyleLoader = require("../loaders/vue-style-loader");

module.exports = sourceMap => {
  return {
    test: /\.css$/,
    use: [vueStyleLoader(sourceMap), cssLoader(sourceMap, 1), postcssLoader(sourceMap)]
  };
};
