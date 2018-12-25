const DEFAULT_OPTIONS = {
  prefix: "/odata",
};

export default class URLBuilder {
  constructor(entity, options) {
    let opts = Object.assign({}, DEFAULT_OPTIONS, options);

    this._baseUrl = `${opts.prefix}/${entity}`;
  }

  withSearch(query) {
    if (query) {
      this._query = `$filter=contains(Search, '${encodeURIComponent(query.toUpperCase())}')`;
    }
    return this;
  }

  expand(values) {
    this._expand = `$expand=${values}`;
    return this;
  }

  orderBy(expression) {
    this._order = `$orderby=${expression}`;
    return this;
  }

  page(pageNumber, pageSize) {
    this._pagination = `$skip=${pageSize * (pageNumber - 1)}&$top=${pageSize}&$count=true`;
    return this;
  }

  top(count) {
    this._top = `$top=${count}`;
    return this;
  }

  build() {
    let url = this._baseUrl;
    let params = [];

    if (this._expand) {
      params.push(this._expand);
    }

    if (this._query) {
      params.push(this._query);
    }

    if (this._order) {
      params.push(this._order);
    }

    if (this._pagination) {
      params.push(this._pagination);
    } else if (this._top) {
      params.push(this._top);
    }

    params.forEach((value, i) => (url = url + (i == 0 ? "?" : "&") + value));

    return url;
  }
}
