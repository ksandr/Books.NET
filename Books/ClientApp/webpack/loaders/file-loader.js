module.exports = outputPath => {
  return {
    loader: "file-loader",
    options: {
      outputPath: outputPath,
      name: "[name].[hash].[ext]"
    }
  };
};
