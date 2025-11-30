import { Button } from '@mui/material';
import { Login as LoginIcon } from '@mui/icons-material';
import { useAuth } from '../../lib/hooks/auth/useAuth';

export const LoginButton = () => {
  const { login, isLoading } = useAuth();

  return (
    <Button
      variant="contained"
      startIcon={<LoginIcon />}
      onClick={login}
      disabled={isLoading}
    >
      Log In
    </Button>
  );
};
