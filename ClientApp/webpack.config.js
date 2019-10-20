const path = require("path");
const webpack = require("webpack");

const {CleanWebpackPlugin} = require("clean-webpack-plugin");
const CompressionPlugin = require("compression-webpack-plugin");
const CopyWebpackPlugin = require("copy-webpack-plugin");
const MiniCssExtractPlugin = require("mini-css-extract-plugin");
const OptimizeCSSAssetsPlugin = require("optimize-css-assets-webpack-plugin");
const VueLoaderPlugin = require("vue-loader/lib/plugin");

const optimizationPreset = require("./webpack/presets/optimization");

const cssRule = require("./webpack/rules/css");
const fontsRule = require("./webpack/rules/fonts");
const imagesRule = require("./webpack/rules/images");
const jsRule = require("./webpack/rules/js");
const scssRule = require("./webpack/rules/scss");
const vueRule = require("./webpack/rules/vue");

const htmlWebpackPlugin = require("./webpack/plugins/htmlWebpackPlugin");
const webpackDefinePlugin = require("./webpack/plugins/webpackDefinePlugin");

const appVersion = require("./webpack/app-version");

module.exports = (env, argv) => {
  env = env || {};
  argv = argv || {};

  const mode = argv.mode || "development";

  const DEBUG = mode !== "production";

  const APP_ENV = env.APP_ENV || "development"; //APP_ENV = development|production

  let config = {
    mode: mode,
    context: __dirname,

    entry: {
      index: "./src/index.js",
    },

    output: {
      path: path.resolve(__dirname, "./dist"),
      publicPath: "/",
      filename: DEBUG ? "js/[name].[hash].js" : "js/[name].[chunkhash].js",
    },
    devtool: DEBUG ? "source-map" : false,

    optimization: optimizationPreset(),

    resolve: {
      extensions: [".js", ".vue"],
      modules: ["node_modules"],
    },

    module: {
      rules: [vueRule(), jsRule([/node_modules/]), cssRule(DEBUG), scssRule(DEBUG), imagesRule(), fontsRule()],
    },

    plugins: [
      // process.env.NODE_ENV value is set by Webpack according to config.mode
      webpackDefinePlugin({
        "process.env.APP_VERSION": appVersion(APP_ENV),
      }),

      new webpack.IgnorePlugin(/^\.\/locale$/, /moment$/),

      new webpack.ProvidePlugin({
        $: "jquery",
        jQuery: "jquery",
      }),

      new VueLoaderPlugin(),

      htmlWebpackPlugin("./src/index.html", "index.html", DEBUG),

      new CopyWebpackPlugin([{from: "src/assets", to: "assets"}]),

      ...(() =>
        DEBUG
          ? [
              new webpack.HotModuleReplacementPlugin(),
              new CompressionPlugin({algorithm: "gzip", cache: true, minRatio: 1.0, threshold: 20, test: /\.(css|html|js)$/}),
            ]
          : [
              new CleanWebpackPlugin(),
              new MiniCssExtractPlugin({filename: "css/[name].[chunkhash].css"}),
              new OptimizeCSSAssetsPlugin(),
              new CompressionPlugin({algorithm: "gzip", cache: true, minRatio: 1.0, threshold: 20, test: /\.(css|html|js)$/}),
            ])(),
    ],
  };

  return config;
};
