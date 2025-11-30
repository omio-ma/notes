import { Button } from '@mui/material';
import { Logout as LogoutIcon } from '@mui/icons-material';
import { useAuth } from '../../lib/hooks/auth/useAuth';

export const LogoutButton = () => {
  const { logout, isLoading } = useAuth();

  return (
    <Button
      variant="outlined"
      startIcon={<LogoutIcon />}
      onClick={logout}
      disabled={isLoading}
    >
      Log Out
    </Button>
  );
};
