const autoprefixer = require("autoprefixer");

module.exports = sourceMap => {
  return {
    loader: "postcss-loader",
    options: {
      sourceMap: sourceMap,
      plugins: () => autoprefixer()
    }
  };
};
