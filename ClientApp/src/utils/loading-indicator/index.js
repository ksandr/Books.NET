import "./style.scss";

const DEFAULT_DELAY = 200;

let element = document.createElement("div");
element.className = "loading-indicator";

let bar = document.createElement("div");
bar.className = "loading-indicator__bar";
element.appendChild(bar);

let spinner = document.createElement("div");
spinner.className = "loading-indicator__spinner circle";
for (let i = 0; i < 6; i++) {
  spinner.appendChild(document.createElement("div"));
}
element.appendChild(spinner);

if (document.body.childElementCount > 0) {
  document.body.insertBefore(element, document.body.firstChild);
} else {
  document.body.appendChild(element);
}

let timeout;

export default {
  delay: DEFAULT_DELAY,

  start() {
    timeout = setTimeout(() => {
      element.style.display = "block";
    }, this.delay);
  },

  stop() {
    clearTimeout(timeout);
    element.style.display = "none";
  },
};
