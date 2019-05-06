/* spell-checker:ignore woff woff2 */
const fileLoader = require("../loaders/file-loader");

module.exports = outputPath => {
  let path = outputPath || "assets/fonts";

  return {
    test: /\.(woff|woff2|eot|ttf|otf)$/,
    use: [fileLoader(path)]
  };
};
