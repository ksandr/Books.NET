module.exports = appEnvironment => {
  const now = new Date();
  const yyyy = now.getFullYear().toString();
  const MM = (now.getMonth() + 1).toString().padStart(2, "0");
  const dd = now
    .getDate()
    .toString()
    .padStart(2, "0");

  const env = appEnvironment.substring(0, 1) || "z";

  return `${yyyy}${MM}${dd}-${env}`;
};
