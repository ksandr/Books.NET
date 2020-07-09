const fileLoader = require("../loaders/file-loader");

module.exports = outputPath => {
  let path = outputPath || "assets/img";

  return {
    test: /\.(png|svg|jpg|gif|ico)$/,
    use: [fileLoader(path)]
  };
};
