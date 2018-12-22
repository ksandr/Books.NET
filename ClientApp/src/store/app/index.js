const states = {
  NAVIGATE: "navigate",
  LOADING: "loading",
  LOADED: "loaded",
  NO_DATA: "no-data",
  ERROR: "error",
};

export default {
  namespaced: true,
  state: {
    state: "none",
  },
  getters: {
    allowNavigation: state => state.state != states.NAVIGATE && state.state != states.LOADING,
    isLoading: state => state.state == states.LOADING,
    isLoaded: state => state.state == states.LOADED,
    isNoData: state => state.state == states.NO_DATA,
  },
  mutations: {
    navigate(state) {
      state.state = states.NAVIGATE;
    },
    loading(state) {
      state.state = states.LOADING;
    },
    loaded(state) {
      state.state = states.LOADED;
    },
    noData(state) {
      state.state = states.NO_DATA;
    },
    error(state) {
      state.state = states.ERROR;
    },
  },
};
