import { Avatar, Box, Typography } from '@mui/material';
import { useAuth } from '../../lib/hooks/auth/useAuth';

export const UserProfile = () => {
  const { user, isAuthenticated } = useAuth();

  if (!isAuthenticated || !user) {
    return null;
  }

  return (
    <Box display="flex" alignItems="center" gap={2}>
      <Avatar src={user.picture} alt={user.name} />
      <Box>
        <Typography variant="body1" fontWeight="medium">
          {user.name}
        </Typography>
        <Typography variant="body2" color="text.secondary">
          {user.email}
        </Typography>
      </Box>
    </Box>
  );
};
