const axios = require('axios')
const TokenService = require('../tokenService')

const client = axios.create({
  baseURL: `${process.env.DATAVERSE_BASE_URL}/api/data/v9.1`,
  headers: {
    'Accept': 'application/json',
    'content-type': 'application/json; charset=utf-8',
    'OData-MaxVersion': '4.0',
    'OData-Version': '4.0',
    'If-None-Match': 'null',
    //'MSCRM.SuppressDuplicateDetection': false
  },
})

class DataverseClient {

  constructor() {
    this.retried = false
  }

  async setToken() {
    const token = await TokenService.getToken()
    client.defaults.headers.common['Authorization'] = `Bearer ${token}`
  }

  async execute(config) {
    await this.setToken()
    try {
      const result = await client(config)
      this.retried = false
      return result
    }
    catch (err) {
      if (!this.retried && (err.response.status === 401 || err.response.status === 403)) {
        this.retried = true
        await TokenService.refreshToken()
        const result = await this.execute(config)
        return result
      }
      else {
        throw err
      }
    }
  }

}


module.exports = new DataverseClient()