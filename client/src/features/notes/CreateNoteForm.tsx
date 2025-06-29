import { useState } from "react";
import { useNavigate } from "react-router";
import Layout from "../../layout/Layout";
import { useCreateNote } from "../../lib/hooks/notes/useCreateNote";

function CreateNoteForm() {
  const [title, setTitle] = useState("");
  const [content, setContent] = useState("");

  const navigate = useNavigate();
  const { mutate } = useCreateNote();

  return (
    <Layout>
      <form
        onSubmit={(e) => {
          e.preventDefault();
          mutate(
            { title, content },
            {
              onSuccess: () => navigate("/")
            }
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
          data-testid="create-note-form-title"
        />
        <textarea
          value={content}
          onChange={(e) => setContent(e.target.value)}
          className="w-full p-2 border border-gray-300 rounded"
          rows={5}
          placeholder="Content"
          data-testid="create-note-form-content"
        />
        <div className="flex justify-between">
          <button
            type="button"
            onClick={() => navigate("/")}
            className="px-4 py-2 bg-gray-300 text-gray-800 rounded cursor-pointer"
            data-testid="create-note-form-back-button"
          >
            Back
          </button>
          <button
            type="submit"
            className="px-4 py-2 bg-green-600 text-white rounded cursor-pointer"
            data-testid="create-note-form-save-button"
          >
            Save
          </button>
        </div>
      </form>
    </Layout>
  );
}

export default CreateNoteForm;
