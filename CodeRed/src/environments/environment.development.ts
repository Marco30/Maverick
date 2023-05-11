const BaseUrl = 'https://localhost:7125/wargames';
export const environment = {
  production: true,
  authenticate_url: `${BaseUrl}/authenticate`,
  reset_password_request: `${BaseUrl}/resetPasswordRequest`,
  reset_password: `${BaseUrl}/resetPassword`,
  refreshToken_url: `${BaseUrl}/newToken`,
  register_url: `${BaseUrl}/registerUser`,
  ask_question: `${BaseUrl}/askquestion`,
  get_ListOfconversations: `${BaseUrl}/getconversations`,
  get_conversation: `${BaseUrl}/getconversation`,
  changeconversation_name: `${BaseUrl}/changeconversationname`,
  get_user_data_from_security_number: `${BaseUrl}/getUserDataFromSecurityNumber`,
};
