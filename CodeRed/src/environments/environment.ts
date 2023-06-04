const BaseUrl = 'https://localhost:7125/wargames';
export const environment = {
  production: false,
  authenticate_url: `${BaseUrl}/authenticate`,
  reset_password_request: `${BaseUrl}/resetPasswordRequest`,
  reset_password: `${BaseUrl}/resetPassword`,
  refresh_token_url: `${BaseUrl}/newToken`,
  register_url: `${BaseUrl}/registerUser`,
  ask_question: `${BaseUrl}/askquestion`,
  get_list_of_conversations: `${BaseUrl}/getconversations`,
  get_list_of_saved_conversations: `${BaseUrl}/getlibraryconversations`,
  get_conversation: `${BaseUrl}/getconversation`,
  delete_conversation: `${BaseUrl}/deleteconversation`,
  changeconversation_name: `${BaseUrl}/changeconversationname`,
  get_user_data_from_security_number: `${BaseUrl}/getUserDataFromSecurityNumber`,
};
