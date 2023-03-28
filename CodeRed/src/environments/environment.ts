const BaseUrl = "https://localhost:44365/miraclemile";
export const environment = {
    production: false,
    authenticate_url: `${BaseUrl}/authenticate`,
    resetPasswordRequest: `${BaseUrl}/resetPasswordRequest`,
    resetPassword: `${BaseUrl}/resetPassword`,
    refreshToken_url: `${BaseUrl}/newToken`,
    register_url: `${BaseUrl}/registerUser`,
};
