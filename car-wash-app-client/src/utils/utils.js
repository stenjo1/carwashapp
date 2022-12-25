export function isLeapYear(year) {
  if ((!(year % 4) && year % 100) || !(year % 400)) {
    return true;
  } else {
    return false;
  }
}

export function getOpeningHour(workingHours) {
  const index = workingHours.indexOf("h");
  return workingHours.slice(0, index);
}

export function getClosingHour(workingHours) {
  const endIndex = workingHours.lastIndexOf("h");
  const startIndex = workingHours.indexOf("-");
  return workingHours.slice(startIndex + 1, endIndex);
}
