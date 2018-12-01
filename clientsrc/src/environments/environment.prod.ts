export const environment = {
  production: true,
  apiUrl: 'http://178.128.212.67:5001',
  idsApiUrl: 'http://178.128.212.67:61555',
  ids: {
    issuer: 'http://harvey-ids',
    loadDocumentUrl: 'http://178.128.212.67:61555/.well-known/openid-configuration',
    requireHttps: false,
    url: 'http://178.128.212.67:61555',
    clientId: 'harvey-rims-page',
    scope: 'openid profile email phone harvey.rims.api roles'
  }
};
