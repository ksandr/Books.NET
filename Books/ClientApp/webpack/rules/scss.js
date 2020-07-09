const cssLoader = require("../loaders/css-loader");
const postcssLoader = require("../loaders/postcss-loader");
const saasLoader = require("../loaders/saas-loader");
const vueStyleLoader = require("../loaders/vue-style-loader");

module.exports = sourceMap => {
  return {
    test: /\.scss$/,
    use: [vueStyleLoader(sourceMap), cssLoader(sourceMap, 2), postcssLoader(sourceMap), saasLoader(sourceMap)]
  };
};
