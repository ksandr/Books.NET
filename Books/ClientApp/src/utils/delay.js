export default function(callback, pause) {
  return new Promise(resolve => {
    setTimeout(() => {
      resolve(callback());
    }, pause);
  });
}
