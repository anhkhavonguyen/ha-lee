export const environment = {
  production: true,
  apiUrl: 'http://192.168.70.170:5001',
  idsApiUrl: 'http://192.168.70.170:61555',
  ids: {
    issuer: 'http://harvey-ids',
    loadDocumentUrl: 'http://192.168.70.170:61555/.well-known/openid-configuration',
    requireHttps: false,
    url: 'http://192.168.70.170:61555',
    clientId: 'harvey-rims-page',
    scope: 'openid profile email phone harvey.rims.api roles'
  }
};
