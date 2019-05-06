const babelLoader = require("../loaders/babel-loader");
const eslintLoader = require("../loaders/eslint-loader");

module.exports = exclude => {
  let excl = exclude || [/node_modules/];

  return {
    test: /.js$/,
    exclude: excl,
    use: [babelLoader(), eslintLoader()]
  };
};
