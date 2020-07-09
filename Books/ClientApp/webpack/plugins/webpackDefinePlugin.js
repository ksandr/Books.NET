const webpack = require("webpack");

module.exports = definitions => {
  let defs = {};
  Object.keys(definitions).forEach(x => {
    defs[x] = JSON.stringify(definitions[x]);
  });

  return new webpack.DefinePlugin(defs);
};
