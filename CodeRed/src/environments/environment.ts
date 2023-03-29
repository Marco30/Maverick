const BaseUrl = 'https://localhost:44365/miraclemile';
export const environment = {
  production: false,
  authenticate_url: `${BaseUrl}/authenticate`,
  reset_password_request: `${BaseUrl}/resetPasswordRequest`,
  reset_password: `${BaseUrl}/resetPassword`,
  refreshToken_url: `${BaseUrl}/newToken`,
  register_url: `${BaseUrl}/registerUser`,
  get_user_data_from_security_number: `${BaseUrl}/getUserDataFromSecurityNumber`,
};
