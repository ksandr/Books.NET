const MiniCssExtractPlugin = require("mini-css-extract-plugin");

module.exports = debug => {
  if (debug) {
    return {
      loader: "vue-style-loader"
    };
  }

  return MiniCssExtractPlugin.loader;
};
