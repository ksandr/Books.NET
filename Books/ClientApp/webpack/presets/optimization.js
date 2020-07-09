const TerserPlugin = require("terser-webpack-plugin");

module.exports = () => {
  return {
    minimizer: [
      new TerserPlugin({
        cache: true,
        parallel: true,
        sourceMap: false
      })
    ],
    runtimeChunk: "single",
    splitChunks: {
      cacheGroups: {
        vendors: {
          test: /[\\/](node_modules)[\\/]/,
          name: "vendors",
          chunks: "all"
        }
      }
    }
  };
};
