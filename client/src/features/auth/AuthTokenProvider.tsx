import { useEffect } from 'react';
import { useAuth0 } from '@auth0/auth0-react';
import { setTokenGetter } from '../../lib/agent';

interface AuthTokenProviderProps {
  children: React.ReactNode;
}

export const AuthTokenProvider = ({ children }: AuthTokenProviderProps) => {
  const { getAccessTokenSilently, isAuthenticated } = useAuth0();

  useEffect(() => {
    if (isAuthenticated) {
      setTokenGetter(async () => {
        try {
          return await getAccessTokenSilently();
        } catch (error) {
          console.error('Error getting access token:', error);
          return '';
        }
      });
    }
  }, [getAccessTokenSilently, isAuthenticated]);

  return <>{children}</>;
};
