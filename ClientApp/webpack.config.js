const CleanWebpackPlugin = require("clean-webpack-plugin");
const CopyWebpackPlugin = require("copy-webpack-plugin");
const HtmlWebpackPlugin = require("html-webpack-plugin");
const MiniCssExtractPlugin = require("mini-css-extract-plugin");
const OptimizeCSSAssetsPlugin = require("optimize-css-assets-webpack-plugin");
const UglifyJSPlugin = require("uglifyjs-webpack-plugin");
const VueLoaderPlugin = require("vue-loader/lib/plugin");
const autoprefixer = require("autoprefixer");
const path = require("path");
const webpack = require("webpack");

module.exports = env => {
  env = env || {
    NODE_ENV: "development"
  };
  const debug = env.NODE_ENV !== "production";

  let config = {
    mode: debug ? "development" : "production",
    context: __dirname,

    entry: {
      index: "./src/index.js"
    },

    output: {
      path: path.resolve(__dirname, "./dist"),
      publicPath: "/",
      filename: debug ? "js/[name].[hash].js" : "js/[name].[chunkhash].js"
    },
    devtool: debug ? "source-map" : false,

    optimization: {
      runtimeChunk: "single",
      splitChunks: {
        cacheGroups: {
          commons: {
            test: /[\\/](node_modules)[\\/]/,
            name: "vendors",
            chunks: "all"
          }
        }
      }
    },

    resolve: {
      extensions: [".js", ".vue"],
      modules: ["node_modules"]
    },

    module: {
      rules: [{
          test: /\.vue$/,
          loader: "vue-loader",
          options: {
            loaders: {
              scss: "vue-style-loader!css-loader!sass-loader"
            }
          }
        },
        {
          test: /.js$/,
          exclude: /node_modules/,
          use: ["babel-loader?cacheDirectory=true", "eslint-loader"]
        },
        {
          test: /\.scss$/,
          use: [
            debug ? {
              loader: "vue-style-loader",
              options: {
                sourceMap: debug
              }
            } :
            MiniCssExtractPlugin.loader,
            {
              loader: "css-loader",
              options: {
                sourceMap: debug
              }
            },
            {
              loader: "postcss-loader",
              options: {
                sourceMap: debug,
                plugins: () =>
                  autoprefixer({
                    browsers: ["last 3 versions", "> 1%"]
                  })
              }
            },
            {
              loader: "sass-loader",
              options: {
                sourceMap: debug
              }
            }
          ]
        },
        {
          test: /\.(png|svg|jpg|gif|ico)$/,
          use: [{
            loader: "file-loader",
            options: {
              outputPath: "assets/img/",
              name: "[name].[hash].[ext]"
            }
          }]
        },
        {
          test: /\.(woff|woff2|eot|ttf|otf)$/,
          use: [{
            loader: "file-loader",
            options: {
              outputPath: "assets/fonts/",
              name: "[name].[hash].[ext]"
            }
          }]
        }
      ]
    },

    plugins: [
      new webpack.DefinePlugin({
        "process.env.NODE_ENV": debug ? JSON.stringify("development") : JSON.stringify("production")
      }),

      new webpack.ProvidePlugin({
        $: "jquery",
        jQuery: "jquery"
      }),
      new CleanWebpackPlugin([path.resolve(__dirname, "./dist")]),
      new VueLoaderPlugin(),
      new HtmlWebpackPlugin({
        template: "./src/index.html",
        filename: "index.html",
        minify: debug ?
          false : {
            collapseWhitespace: true
          }
      }),
      new CopyWebpackPlugin([{
        from: "src/assets",
        to: "."
      }])
    ]
  };

  if (debug) {
    config.plugins = config.plugins.concat([new webpack.NamedModulesPlugin(), new webpack.HotModuleReplacementPlugin()]);
  } else {
    config.plugins = config.plugins.concat([
      new webpack.HashedModuleIdsPlugin(),
      new MiniCssExtractPlugin({
        filename: "css/[name].[chunkhash].css"
      }),
      new OptimizeCSSAssetsPlugin(),
      new UglifyJSPlugin({
        cache: true,
        parallel: true,
        sourceMap: false
      })
    ]);
  }

  return config;
};