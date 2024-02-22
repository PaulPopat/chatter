/**
 * @param {string} token
 * @param {import('axios').AxiosRequestConfig} config 
 * @returns {import('axios').AxiosRequestConfig}
 */
module.exports = function authorised(token, config = {}) {
  return {
    ...config,
    headers: {
      ...(config?.headers ?? {}),
      Authorization: `Bearer ${token}`
    }
  }
}