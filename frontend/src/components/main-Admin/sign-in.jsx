import { Helmet } from 'react-helmet-async';

import { CONFIG } from '../config-global'; // Adjust path if needed

import { SignInView } from '../sections/auth'; // Adjust path if needed

// ----------------------------------------------------------------------

export default function Page() {
  return (
    <>
      <Helmet>
        <title>{`Sign in - ${CONFIG.appName}`}</title>
      </Helmet>

      <SignInView />
    </>
  );
}
