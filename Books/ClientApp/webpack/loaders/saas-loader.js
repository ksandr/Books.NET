module.exports = sourceMap => {
  return {
    loader: "sass-loader",
    options: {
      sourceMap: sourceMap
    }
  };
};
