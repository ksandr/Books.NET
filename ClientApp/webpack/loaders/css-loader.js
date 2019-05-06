module.exports = (sourceMap, importLoaders = 0) => {
  return {
    loader: "css-loader",
    options: {
      importLoaders: importLoaders,
      sourceMap: sourceMap
    }
  };
};
