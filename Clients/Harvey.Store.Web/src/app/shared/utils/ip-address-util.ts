export class IpAddressUtil {
  static GetIpAddress() {
    let ipaddress = null;
    const publicIp = require('public-ip');
    publicIp.v4().then(ip => {
      ipaddress = ip;
    });
    return ipaddress;
  }
}
