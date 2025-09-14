const msal = require('@azure/msal-node')
const diskCache = require('./diskCache')

const clientConfig = {
  auth: {
    authority: 'https://login.microsoftonline.com/d4586377-0b94-4f2d-b20f-a4a8591bb463',
    clientId: 'ae66f843-9fe3-47aa-bf09-ac9011c05d48',
    clientSecret: process.env.DATAVERSE_CLIENT_SECRET,
  },
}

class TokenService {

  constructor() {
    this.token = ''
    this.confidentialClientApplication = new msal.ConfidentialClientApplication(clientConfig)
    this.clientCredentialRequest = {
      scopes: [`${process.env.DATAVERSE_BASE_URL}/.default`],
      azureRegion: null,
      skipCache: false,
    }
  }

  async getToken() {
    if (!this.token) {
      await this.acquireToken()
    }
    return this.token
  }

  async acquireToken() {
    try {
      const savedToken = await diskCache.readFile(process.env.TOKEN_LOCATION)
      this.token = savedToken
    }
    catch (err) {
      try {
        await this.generateAndCacheToken()
      }
      catch (err) {
        return err
      }
    }
  }

  async generateAndCacheToken() {
    const result = await this.confidentialClientApplication.acquireTokenByClientCredential(this.clientCredentialRequest)
    this.token = result.accessToken
    diskCache.writeFile(process.env.TOKEN_LOCATION, result.accessToken)
  }

  async refreshToken() {
    try {
      await this.generateAndCacheToken()
    }
    catch (err) {
      return err
    }
  }

}

module.exports = new TokenService()