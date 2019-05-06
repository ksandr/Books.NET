const HtmlWebpackPlugin = require("html-webpack-plugin");

module.exports = (template, filename, debug, options) => {
  let opts = {
    template: template,
    filename: filename
  };

  if (!debug) {
    opts.minify = {
      collapseWhitespace: true,
      removeComments: true
    };
  } else {
    opts.minify = false;
  }

  let pluginOpts = Object.assign({}, opts, options);
  return new HtmlWebpackPlugin(pluginOpts);
};
