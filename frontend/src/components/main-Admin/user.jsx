import { Helmet } from 'react-helmet-async';

import { CONFIG } from '../config-global'; // Adjust path if needed

import { UserView } from '../sections/user/view'; // Adjust path if needed

// ----------------------------------------------------------------------

export default function Page() {
  return (
    <>
      <Helmet>
        <title>{`Users - ${CONFIG.appName}`}</title>
      </Helmet>

      <UserView />
    </>
  );
}
