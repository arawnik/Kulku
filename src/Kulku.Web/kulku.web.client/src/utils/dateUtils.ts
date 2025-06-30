/**
 * Extracts the year from a given date.
 *
 * @param {string | Date} date - The date in ISO format or Date object.
 * @returns {string} The year as a four-digit string.
 */
export const getYear = (date: string | Date): string => {
  return new Date(date).getFullYear().toString()
}

/**
 * Extracts the year and month from a given date in "MM/yyyy" or "MM-yyyy" format.
 *
 * @param {string | Date} date - The date in ISO format or Date object.
 * @param {boolean} [useHyphen=false] - If true, returns "MM-yyyy" format; otherwise, "MM/yyyy".
 * @returns {string} The year and month in the specified format.
 */
export const getYearMonth = (date: string | Date, useHyphen: boolean = false): string => {
  const d = new Date(date)
  const month = String(d.getMonth() + 1).padStart(2, '0')
  const year = d.getFullYear()
  return `${month}${useHyphen ? '-' : '/'}${year}`
}

/**
 * Generates a formatted date range string for a given start and end date.
 * If no end date is provided, defaults to "Present".
 *
 * @param {string | Date} startDate - The start date in ISO format or Date object.
 * @param {string | Date | null} endDate - The end date in ISO format or Date object, or null for ongoing.
 * @param {boolean} [useHyphen=false] - If true, formats dates as "MM-yyyy"; otherwise, "MM/yyyy".
 * @param {string} [presentText='Present'] - Translated "Present" text to show when ongoing.
 * @returns {string} Formatted date range with Present text if ongoing.
 */
export const formatEraText = (
  startDate: string | Date,
  endDate: string | Date | null = null,
  useHyphen: boolean = false,
  presentText: string = 'Present'
): string => {
  const startStr = startDate ? getYearMonth(startDate, useHyphen) : ''
  const endStr = endDate ? getYearMonth(endDate, useHyphen) : presentText

  return `${startStr} - ${endStr}`
}
