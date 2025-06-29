import { useEffect, useState } from "react";
import { useNavigate, useParams } from "react-router-dom";
import { useNotes } from "../../lib/hooks/notes/useNotes";
import { useUpdateNote } from "../../lib/hooks/notes/useUpdateNote";
import Layout from "../../layout/Layout";

function EditNoteForm() {
  const { id } = useParams<{ id: string }>();
  const { data: notes } = useNotes();
  const { mutate } = useUpdateNote();
  const navigate = useNavigate();

  const note = notes?.find((n) => n.id.toString() === id);
  const [title, setTitle] = useState("");
  const [content, setContent] = useState("");

  useEffect(() => {
    if (note) {
      setTitle(note.title);
      setContent(note.content);
    }
  }, [note]);

  if (!note) return <p>Note not found</p>;

  return (
    <Layout>
      <form
        onSubmit={(e) => {
          e.preventDefault();
          mutate(
            { id: note.id, data: { title, content } },
            { onSuccess: () => navigate("/") }
          );
        }}
        className="space-y-4 animate-fade-in"
      >
        <input
          type="text"
          value={title}
          onChange={(e) => setTitle(e.target.value)}
          className="w-full p-2 border border-gray-300 rounded"
          placeholder="Title"
          data-testid="note-form-title"
        />
        <textarea
          value={content}
          onChange={(e) => setContent(e.target.value)}
          className="w-full p-2 border border-gray-300 rounded"
          rows={5}
          placeholder="Content"
          data-testid="note-form-content"
        />
        <div className="flex justify-between">
          <button
            type="button"
            onClick={() => navigate("/")}
            className="px-4 py-2 bg-gray-300 text-gray-800 rounded cursor-pointer"
            data-testid="note-form-back-button"
          >
            Back
          </button>
          <button
            data-testid="note-form-save-button"
            type="submit"
            className="px-4 py-2 bg-green-600 text-white rounded cursor-pointer"
          >
            Save
          </button>
        </div>
      </form>
    </Layout>
  );
}

export default EditNoteForm;
