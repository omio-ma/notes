import { useAuth } from '../../lib/hooks/auth/useAuth';
import { LoginButton } from './LoginButton';
import { LogoutButton } from './LogoutButton';

export const AuthButton = () => {
  const { isAuthenticated, isLoading } = useAuth();

  if (isLoading) {
    return null;
  }

  return isAuthenticated ? <LogoutButton /> : <LoginButton />;
};
