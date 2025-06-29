import { CircularProgress } from "@mui/material";
import { useNotes } from "../../lib/hooks/notes/useNotes";
import NotesList from "../notes/NotesList";
import Layout from "../../layout/Layout";

function HomePage() {
  const { data: notes, isLoading, error } = useNotes();

  return (
    <Layout>
      <h1 className="text-3xl font-bold mb-6 text-orange-600">My Notes</h1>
      {isLoading ? (
        <CircularProgress />
      ) : error ? (
        <p className="text-orange-600">Failed to load notes.</p>
      ) : (
        <NotesList notes={notes} />
      )}
    </Layout>
  );
}

export default HomePage;
