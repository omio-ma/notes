import axios from "axios";

const agent = axios.create({
  baseURL: import.meta.env.VITE_API_URL
});

// Token getter function that will be set by the Auth provider
let getAccessToken: (() => Promise<string>) | null = null;

export const setTokenGetter = (getter: () => Promise<string>) => {
  getAccessToken = getter;
};

// Add request interceptor to include the access token
agent.interceptors.request.use(
  async (config) => {
    if (getAccessToken) {
      try {
        const token = await getAccessToken();
        if (token) {
          config.headers.Authorization = `Bearer ${token}`;
        }
      } catch (error) {
        console.error('Error getting access token:', error);
      }
    }
    return config;
  },
  (error) => {
    return Promise.reject(error);
  }
);

export default agent;
