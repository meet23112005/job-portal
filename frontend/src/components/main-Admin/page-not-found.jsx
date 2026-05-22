import { Helmet } from 'react-helmet-async';

import { CONFIG } from '../config-global'; // Adjust path if needed

import { NotFoundView } from '../sections/error'; // Adjust path if needed

// ----------------------------------------------------------------------

export default function Page() {
  return (
    <>
      <Helmet>
        <title>{`404 page not found! | Error - ${CONFIG.appName}`}</title>
      </Helmet>

      <NotFoundView />
    </>
  );
}
